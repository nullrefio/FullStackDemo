import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WidgetListComponent } from './entity-management/widget/widget-list/widget-list.component';
import { WidgetEditComponent } from './entity-management/widget/widget-edit/widget-edit.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { HomePageComponent } from './home-page/home-page.component';

const routes: Routes = [
  {
    path: '',
    data: { title: 'Demo' },
    component: HomePageComponent
  },
  {
    path: 'widgets',
    data: { title: 'Widgets' },
    children: [
      { path: '', component: WidgetListComponent, data: { title: '' } },
      { path: 'create', component: WidgetEditComponent, data: { mode: 'create', title: 'New Widget' } },
      { path: ':id', component: WidgetEditComponent, data: { mode: 'edit', title: 'Edit' } }
    ]
  },
  { path: '**', component: PageNotFoundComponent, data: { title: 'Page Not Found' } }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { onSameUrlNavigation: 'reload' })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
