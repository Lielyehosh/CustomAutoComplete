import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {FormBuilder, FormGroup} from "@angular/forms";
import {BehaviorSubject, Observable, Subject} from "rxjs";
import {AppService} from "../../../api/services/app.service";
import {autocomplete} from "../../../api/utils";
import {FieldChoice} from "../../../api/models/fieldChoice";
import {shareReplay, takeUntil} from "rxjs/operators";

@Component({
  selector: 'app-auto-complete',
  templateUrl: './auto-complete.component.html',
  styleUrls: ['./auto-complete.component.scss']
})
export class AutoCompleteComponent implements OnInit, OnDestroy {
  private destroy$: Subject<void> = new Subject<void>();


  @Input('table') table: string;
  queryObs = new BehaviorSubject<string>('');
  results$: Observable<Array<FieldChoice>>;
  stateForm: FormGroup;
  isLoading = true;
  showDropDown = false;
  currIndex: number;
  private limit: number = 8;


  constructor(private fb: FormBuilder, protected appService: AppService) {
    this.initForm();
    this.isLoading = false;
  }

  initForm(): FormGroup {
    return this.stateForm = this.fb.group({
      search: ['']
    });
  }

  ngOnInit(): void {
    this.results$ = this.queryObs.pipe(
      autocomplete(100, ((substring) => this.appService.getAutoComplete(this.table, substring, this.limit))),
      takeUntil(this.destroy$),
      shareReplay<Array<FieldChoice>>(),
    );
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  selectValue(value: FieldChoice, index: number) {
    debugger;
    this.stateForm?.patchValue({"search": value.label});
    this.inputChanged(value.label);
    this.currIndex = index;
  }

  closeDropDown() {
    this.showDropDown = false
  }

  openDropDown() {
    this.showDropDown = true;
    this.currIndex = 0;
  }

  toggleDropDown() {
    this.showDropDown = !this.showDropDown;
  }

  getSearchValue() {
    return this.stateForm?.value.search;
  }

  inputChanged(value: string) {
    this.queryObs.next(value);
    this.openDropDown();
  }

  incSelectedValue() {
    this.currIndex = (this.currIndex + 1) % this.limit;
    console.log(this.currIndex + "after increase");
  }

  decSelectedValue() {
    this.currIndex = (this.currIndex - 1) % this.limit;
    console.log(this.currIndex + "after decrease");
  }
}
