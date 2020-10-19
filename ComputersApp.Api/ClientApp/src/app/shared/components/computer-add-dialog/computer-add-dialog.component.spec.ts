/// <reference types="Jasmine" />

import { ICpu } from './../../../core/models/cpu';
import { EMPTY, of } from 'rxjs';
import { ComputerService } from 'src/app/core/services/computer.service';
import { CpuService } from 'src/app/core/services/cpu.service';
import { ComputerAddDialogComponent } from './computer-add-dialog.component';
describe('ComputerAddDialogComponent', () => {
    let component: ComputerAddDialogComponent;
    let cpuService: CpuService;
    let computerService: ComputerService;
    let getAllSpy: any;

    beforeEach(() => {
        // Arrange
        cpuService = new CpuService(null);
        computerService = new ComputerService(null);
        component = new ComputerAddDialogComponent(computerService, cpuService);
        getAllSpy = spyOn(cpuService, 'getAll').and.callFake(() => {
            return EMPTY;
        });
    });

    it('should create form with 5 control', () => {
        // Assert
        expect(component.addComputerForm.contains('name')).toBeTruthy();
        expect(component.addComputerForm.contains('ssdAmount')).toBeTruthy();
        expect(component.addComputerForm.contains('ramAmount')).toBeTruthy();
        expect(component.addComputerForm.contains('osType')).toBeTruthy();
        expect(component.addComputerForm.contains('cpu')).toBeTruthy();
    });

    it('should mark name as invalid if empty value', () => {
        // Arrange
        const control = component.addComputerForm.get('name');

        // Act
        control.setValue('');

        // Assert
        expect(control.valid).toBeFalsy();
    });

    it('should mark name as invalid if value greater than 50', () => {
        // Arrange
        const control = component.addComputerForm.get('name');
        const invalidName = 'gggggggdhdgdgdgsheyhdgdgdgdhdgdhdgdhdgdhdgdhshshshshshshshshshshshshshshh';

        // Act
        control.setValue(invalidName);

        // Assert
        expect(control.valid).toBeFalsy();
    });

    it('should mark ssdAmount as invalid if empty value', () => {
        // Arrange
        const control = component.addComputerForm.get('ssdAmount');

        // Act
        control.setValue('');

        // Assert
        expect(control.valid).toBeFalsy();
    });

    it('should mark ramAmount as invalid if empty value', () => {
        // Arrange
        const control = component.addComputerForm.get('ramAmount');

        // Act
        control.setValue('');

        // Assert
        expect(control.valid).toBeFalsy();
    });

    it('should getAll cpus in ngOnInit method', () => {
        // Arrange
        const cpu: ICpu = { id: 1, name: 'Hello', corsAmount: 20, frequency: 2500, computers: null };
        getAllSpy.and.callFake(() => {
            return of([cpu]);
        });

        // Act
        component.ngOnInit();

        // Assert
        expect(getAllSpy).toHaveBeenCalled();
        expect(component.cpus.length).toBe(1);
    });

    it('should return from function if form invalid', () => {
        // Arrange
        component.ngOnInit();
        const spy = spyOn(computerService, 'post').and.callFake(() => {
            return EMPTY;
        });
        const control = component.addComputerForm.get('name');

        // Act
        control.setValue('');
        component.onSubmit();

        // Assert
        expect(spy).not.toHaveBeenCalled();

    });

});

