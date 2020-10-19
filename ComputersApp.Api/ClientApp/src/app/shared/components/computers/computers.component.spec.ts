/// <reference types="Jasmine" />

import { ICpu } from './../../../core/models/cpu';
import { CpuService } from './../../../core/services/cpu.service';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { RefDirective } from './../../directives/ref.directive';
import { RegisterComponent } from './../register/register.component';
import { LoginPopupComponent } from './../login-popup/login-popup.component';
import { ComputerEditDialogComponent } from './../computer-edit-dialog/computer-edit-dialog.component';
import { ComputerAddDialogComponent } from './../computer-add-dialog/computer-add-dialog.component';
import { EMPTY, of, throwError } from 'rxjs';
import { IComputer } from 'src/app/core/models/computer';
import { ComputerService } from 'src/app/core/services/computer.service';
import { ComputersComponent } from './computers.component';
import { ComponentFactoryResolver, DebugElement, Type } from '@angular/core';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { BrowserDynamicTestingModule } from '@angular/platform-browser-dynamic/testing';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { APP_BASE_HREF } from '@angular/common';
import { OsType } from 'src/app/core/models/OsType.enum';

describe('ComputersComponent', () => {
  let component: ComputersComponent;
  let computerService: ComputerService;
  let cpuService: CpuService;
  let registerComponent: RegisterComponent;
  let computerAddComponent: ComputerAddDialogComponent;
  let computerEditComponent: ComputerEditDialogComponent;
  let loginComponent: LoginPopupComponent;
  let componentFactoryResolver: ComponentFactoryResolver;
  let computers: IComputer[];
  let getAllSpy: any;
  let deleteSpy: any;


  beforeEach(async(() => {
    // Arrange
    TestBed.configureTestingModule({
      imports: [BrowserModule, FormsModule, HttpClientModule, RouterModule.forRoot([]),
        ReactiveFormsModule],
      declarations: [ComputerAddDialogComponent, ComputerEditDialogComponent,
        LoginPopupComponent, RegisterComponent, RefDirective],
      providers: [{provide: APP_BASE_HREF, useValue: '/'}]
    }).compileComponents();

    TestBed.overrideModule(BrowserDynamicTestingModule, {
      set: {
        entryComponents: [ComputerAddDialogComponent, ComputerEditDialogComponent,
          LoginPopupComponent, RegisterComponent]
      }
    });
    TestBed.compileComponents();

    computers = [
      { id: 1, name: 'Intel Core', ramAmount: 16, ssdAmount: 256 },
      { id: 2, name: 'AMD', ramAmount: 16, ssdAmount: 256 }
    ];
    computerService = new ComputerService(null);
    getAllSpy = spyOn(computerService, 'getAll').and.callFake(() => {
      return of(computers);
    });
    deleteSpy = spyOn(computerService, 'delete').and.callFake(() => {
      computers.pop();
      return of(computers);
    });
    component = new ComputersComponent(computerService, null);


  }));

  it('should create the ComputerAddDialogComponent', () => {
    // Arrange
    const fixture: ComponentFixture<ComputerAddDialogComponent> = TestBed.createComponent(ComputerAddDialogComponent);
    computerAddComponent = fixture.componentInstance;
    componentFactoryResolver = fixture.debugElement.injector.get<ComponentFactoryResolver>(ComponentFactoryResolver as any);

    // Act
    fixture.detectChanges();

    // Assert
    expect(computerAddComponent).toBeTruthy();

    document.body.removeChild(fixture.debugElement.nativeElement);
  });

  it('should create the ComputerEditDialogComponent', () => {
    // Arrange
    const cpus: ICpu[] = [
      { id: 1, name: 'Intel Core', corsAmount: 4, frequency: 3600, computers: null}
    ];
    const fixture: ComponentFixture<ComputerEditDialogComponent> = TestBed.createComponent(ComputerEditDialogComponent);
    computerEditComponent = fixture.componentInstance;
    componentFactoryResolver = fixture.debugElement.injector.get<ComponentFactoryResolver>(ComponentFactoryResolver as any);
    const computer: IComputer =  { id: 1, name: 'Intel Core', ramAmount: 16, ssdAmount: 256, cpu: cpus[0], osType: <number>OsType.Windows};
    computerEditComponent.computer = computer;
    cpuService = fixture.debugElement.injector.get<CpuService>(CpuService as any);
    spyOn(cpuService, 'getAll').and.callFake(() => {
      return of(cpus);
    });

    // Act
    fixture.detectChanges();

    // Assert
    expect(computerEditComponent).toBeTruthy();

    document.body.removeChild(fixture.debugElement.nativeElement);
  });

  it('should create the LoginPopUpComponent', () => {
    // Arrange
    const fixture: ComponentFixture<LoginPopupComponent> = TestBed.createComponent(LoginPopupComponent);
    loginComponent = fixture.componentInstance;
    componentFactoryResolver = fixture.debugElement.injector.get<ComponentFactoryResolver>(ComponentFactoryResolver as any);

    // Act
    fixture.detectChanges();

    // Assert
    expect(loginComponent).toBeTruthy();

    document.body.removeChild(fixture.debugElement.nativeElement);
  });

  it('should create the RegisterComponent', () => {
    // Arrange
    const fixture: ComponentFixture<RegisterComponent> = TestBed.createComponent(RegisterComponent);
    registerComponent = fixture.componentInstance;
    componentFactoryResolver = fixture.debugElement.injector.get<ComponentFactoryResolver>(ComponentFactoryResolver as any);

    // Act
    fixture.detectChanges();

    // Assert
    expect(registerComponent).toBeTruthy();

    document.body.removeChild(fixture.debugElement.nativeElement);
  });

  it('should set computers property with the items returned from the server', () => {
    // Act
    component.ngOnInit();

    // Assert
    expect(component.computers).toBe(computers);
  });

  it('should call the server to delete a computer', () => {
    // Act
    component.deleteComputer(1);

    // Assert
    expect(computers.length).toBe(1);
    expect(getAllSpy).toHaveBeenCalled();
    expect(deleteSpy).toHaveBeenCalledWith(1);
  });

});
