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
        cpuService = new CpuService(null);
        computerService = new ComputerService(null);
        component = new ComputerAddDialogComponent(computerService, cpuService);
        getAllSpy = spyOn(cpuService, 'getAll').and.callFake(() => {
            return EMPTY;
        });
    });

    it('should create form with 5 control', () => {
        expect(component.addComputerForm.contains('name')).toBeTruthy();
        expect(component.addComputerForm.contains('ssdAmount')).toBeTruthy();
        expect(component.addComputerForm.contains('ramAmount')).toBeTruthy();
        expect(component.addComputerForm.contains('osType')).toBeTruthy();
        expect(component.addComputerForm.contains('cpu')).toBeTruthy();
    });

    it('should mark name as invalid if empty value', () => {
        const control = component.addComputerForm.get('name');
        control.setValue('');
        expect(control.valid).toBeFalsy();
    });

    it('should mark name as invalid if value greater than 50', () => {
        const control = component.addComputerForm.get('name');
        const invalidName = 'gggggggdhdgdgdgsheyhdgdgdgdhdgdhdgdhdgdhdgdhshshshshshshshshshshshshshshh';
        control.setValue(invalidName);
        expect(control.valid).toBeFalsy();
    });

    it('should mark ssdAmount as invalid if empty value', () => {
        const control = component.addComputerForm.get('ssdAmount');
        control.setValue('');
        expect(control.valid).toBeFalsy();
    });

    it('should mark ramAmount as invalid if empty value', () => {
        const control = component.addComputerForm.get('ramAmount');
        control.setValue('');
        expect(control.valid).toBeFalsy();
    });

    it('should getAll cpus in ngOnInit method', () => {
        const cpu: ICpu = { id: 1, name: 'Hello', corsAmount: 20, frequency: 2500, computers: null };
        getAllSpy.and.callFake(() => {
            return of([cpu]);
        });
        component.ngOnInit();
        expect(getAllSpy).toHaveBeenCalled();
        expect(component.cpus.length).toBe(1);
    });

    it('should return from function if form invalid', () => {
        component.ngOnInit();
        const spy = spyOn(computerService, 'post').and.callFake(() => {
            return EMPTY;
        });
        const control = component.addComputerForm.get('name');
        control.setValue('');
        component.onSubmit();
        expect(spy).not.toHaveBeenCalled();

    });

});

