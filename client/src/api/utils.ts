import {debounceTime, skip, switchMap, takeUntil} from "rxjs/operators";

export const autocomplete = (time: any, selector: any) => (source$: any) =>
  source$.pipe(
    debounceTime(time),
    // TODO - consider replace with distinctUntilChanged()
    switchMap((...args: any[]) => selector(...args)
        .pipe(
          takeUntil(
            source$
              .pipe(
                skip(1)
              )
          )
        )
    )
  )

