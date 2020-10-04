import { ComputerAddDialogComponent } from './shared/components/computer-add-dialog/computer-add-dialog.component';
import { ComputerEditDialogComponent } from './shared/components/computer-edit-dialog/computer-edit-dialog.component';
import { ComputersComponent } from './shared/components/computers/computers.component';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { RefDirective } from './shared/directives/ref.directive';
import { NavbarComponent } from './shared/components/navbar/navbar.component';
import { LoginComponent } from './shared/components/login/login.component';
import { JwtInterceptor } from './core/interceptors/jwt.interceptor';
import { AuthGuard } from './core/guards/auth.guard';


@NgModule({
  declarations: [
    AppComponent,
    ComputersComponent,
    ComputerEditDialogComponent,
    ComputerAddDialogComponent,
    NavbarComponent,
    LoginComponent,
    RefDirective
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: ComputersComponent, canActivate: [AuthGuard] },
      { path: 'login', component: LoginComponent },
      { path: '**', redirectTo: '', canActivate: [AuthGuard] }
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
],
  entryComponents: [ComputerEditDialogComponent, ComputerAddDialogComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
