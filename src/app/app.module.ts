import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppComponent} from './app.component';
import {AutoCompleteComponent} from './shared/auto-complete/auto-complete.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {ClickOutsideDirective} from './shared/click-outside.directive';
import {SearchFilterPipe} from './shared/pipes/search-filter.pipe';
import {LetterBoldPipe} from './shared/pipes/letter-bold.pipe';
import {HttpClientModule} from "@angular/common/http";


@NgModule({
  declarations: [
    AppComponent,
    AutoCompleteComponent,
    ClickOutsideDirective,
    SearchFilterPipe,
    LetterBoldPipe
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
