import {Component, OnDestroy} from '@angular/core';
import {FieldChoice} from "../api/models/fieldChoice";
import {AppService} from "../api/services/app.service";
import {City} from "../api/models/city";
import {BehaviorSubject, Observable, Subject} from "rxjs";
import {switchMap, takeUntil} from "rxjs/operators";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnDestroy {
  private destroy$: Subject<void> = new Subject<void>();
  title = 'Custom Auto Complete';
  table: string = "city";
  city$: Observable<City>;
  // city: City;
  select$: BehaviorSubject<City> = new BehaviorSubject<City>(null);

  constructor(protected appService: AppService) {
  }

  onSelect($event: FieldChoice) {
    this.city$ = this.select$.pipe(
        switchMap(() => this.appService.getObjectById<City>(this.table, $event.id)),
        takeUntil(this.destroy$)
      );
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
