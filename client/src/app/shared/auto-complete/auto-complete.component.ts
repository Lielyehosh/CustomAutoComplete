import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {FormBuilder, FormGroup} from "@angular/forms";
import {BehaviorSubject, Observable, Subject} from "rxjs";
import {AppService} from "../../../api/services/app.service";
import {autocomplete} from "../../../api/utils";
import {FieldChoice} from "../../../api/models/fieldChoice";
import {takeUntil} from "rxjs/operators";

@Component({
  selector: 'app-auto-complete',
  templateUrl: './auto-complete.component.html',
  styleUrls: ['./auto-complete.component.scss']
})
export class AutoCompleteComponent implements OnInit, OnDestroy {
  @Input('table') table: string;
  queryObs = new BehaviorSubject<string>('');
  results$: Observable<Array<FieldChoice>>;
  private destroy$: Subject<void> = new Subject<void>();
  isLoading = true;
  stateForm: FormGroup;
  showDropDown = false;


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
      autocomplete(100, ((substring) => this.appService.getAutoComplete(this.table, substring))),
      takeUntil(this.destroy$)
    );
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  selectValue(value: FieldChoice) {
    this.stateForm?.patchValue({"search": value.label});
    this.showDropDown = false;
  }
  closeDropDown() {
    this.showDropDown = !this.showDropDown;
  }

  openDropDown() {
    this.showDropDown = false;
  }

  getSearchValue() {
    return this.stateForm?.value.search;
  }

  inputChanged($event: Event) {
    this.queryObs.next(($event.target as HTMLInputElement).value);
  }

  onSearchChanged($event: Event) {
    console.log($event);
  }
}
