import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HttpClientModule} from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ExpenseItemComponent } from './tracker/expense-item/expense-item.component';
import { SearchOptionComponent } from './tracker/search-option/search-option.component';
import { TrackerModule } from './tracker/tracker.module';

@NgModule({
  declarations: [
    AppComponent,
    SearchOptionComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    TrackerModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
