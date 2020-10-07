import { AdminComponent } from './shared/components/admin/admin.component';
import { LoginPopupComponent } from './shared/components/login-popup/login-popup.component';
import { RegisterComponent } from './shared/components/register/register.component';
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
import { Role } from './core/models/role.enum';


@NgModule({
  declarations: [
    AppComponent,
    AdminComponent,
    ComputersComponent,
    ComputerEditDialogComponent,
    ComputerAddDialogComponent,
    LoginPopupComponent,
    RegisterComponent,
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
      { path: 'admin', component: AdminComponent, canActivate: [AuthGuard], data: { roles: [Role.Admin] } },
      { path: '**', redirectTo: '', canActivate: [AuthGuard] }
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
],
  entryComponents: [ComputerEditDialogComponent, ComputerAddDialogComponent,
    RegisterComponent, LoginPopupComponent],
  bootstrap: [AppComponent]
})
export class AppModule { }
