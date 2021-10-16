import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup} from "@angular/forms";
import {BehaviorSubject, Subject} from "rxjs";
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

  @Output('select') selectEvent: EventEmitter<FieldChoice> = new EventEmitter<FieldChoice>();
  @Output('touch') touchEvent: EventEmitter<any> = new EventEmitter<any>();

  @Input('table') table: string;
  queryObs = new BehaviorSubject<string>('');
  stateForm: FormGroup;
  isLoading = true;
  showDropDown = false;
  currIndex: number;
  private limit: number = 8;
  results: Array<FieldChoice>;


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
    this.queryObs.pipe(
      autocomplete(100, ((substring) => this.appService.getAutoComplete(this.table, substring, this.limit))),
      takeUntil(this.destroy$),
      shareReplay<Array<FieldChoice>>(),
    ).subscribe(
      res => {
        this.results = res;
      }
    );
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  selectValue(index: number) {
    if (index >= this.results.length || index < 0)
      return;

    this.queryObs.next(this.results[index].label);
    this.closeDropDown();
    this.stateForm?.patchValue({"search": this.results[index].label});
    this.currIndex = index;
    this.selectEvent.emit(this.results[index]);
  }

  closeDropDown() {
    this.showDropDown = false
  }

  openDropDown() {
    this.showDropDown = true;
    this.currIndex = 0;
    this.touchEvent.emit();
  }

  getSearchValue() {
    return this.stateForm?.value.search;
  }

  inputChanged(value: string) {
    this.queryObs.next(value);
    this.openDropDown();
    this.touchEvent.emit();
  }

  incSelectedValue() {
    this.currIndex = (this.currIndex + 1) % this.results.length;
  }

  decSelectedValue() {
    if (this.currIndex == 0)
      this.currIndex = this.results.length - 1;
    else
      this.currIndex = (this.currIndex - 1) % this.results.length;
  }
}
