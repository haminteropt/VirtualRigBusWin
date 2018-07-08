import { Routes, RouterModule, CanActivate } from '@angular/router';
import { PageNotFoundComponent } from './components/page-not-found-component/page-not-found.component';
import { HomeComponent } from './components/home-component/home.component';

export const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: 'home',
    component: HomeComponent
  },
  { path: '**', component: PageNotFoundComponent }
];
