import { Component, ComponentFactoryResolver, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { IUser } from 'src/app/core/models/user';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { RefDirective } from '../../directives/ref.directive';
import { RegisterComponent } from '../register/register.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  @ViewChild(RefDirective, {static: false}) refDir: RefDirective;
  currentUser: IUser;
  constructor(
    private router: Router,
    private authenticationService: AuthenticationService,
    private resolver: ComponentFactoryResolver
) {}

  ngOnInit() {
    this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
  }

  signIn() {
    this.router.navigate(['/login']);
}

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['/login']);
}

  showRegisterForm() {
    const formFactory = this.resolver.resolveComponentFactory(RegisterComponent);
    const instance = this.refDir.containerRef.createComponent(formFactory).instance;
    instance.onCancel.subscribe(() => {this.refDir.containerRef.clear(); this.ngOnInit(); });
  }

}
