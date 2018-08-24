import { HttpClient, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { appRoutes } from './app.router';
import { ListServicesComponent } from './components/list-services/list-services.component';
import { MaterialModule } from './material.module';
import { PageNotFoundComponent } from './components/page-not-found-component/page-not-found.component';
import { HomeComponent } from './components/home-component/home.component';
import { DirService } from './services/DirService.service';
import { ConfigService } from './services/config-service.service';

@NgModule({
  declarations: [
    AppComponent,
    ListServicesComponent,
    PageNotFoundComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MaterialModule,
    RouterModule.forRoot(appRoutes)
  ],
  providers: [DirService,
    ConfigService,
    HttpClient],
  bootstrap: [AppComponent]
})
export class AppModule { }

