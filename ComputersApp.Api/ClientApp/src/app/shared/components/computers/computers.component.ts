import { OsType } from './../../../core/models/OsType.enum';
import { IComputer } from './../../../core/models/computer';
import { Component, ComponentFactoryResolver, OnInit, ViewChild } from '@angular/core';
import { RefDirective } from '../../directives/ref.directive';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ComputerService } from 'src/app/core/services/computer.service';
import { ComputerEditDialogComponent } from '../computer-edit-dialog/computer-edit-dialog.component';
import { ComputerAddDialogComponent } from '../computer-add-dialog/computer-add-dialog.component';

@Component({
  selector: 'app-computers',
  templateUrl: './computers.component.html',
  styleUrls: ['./computers.component.scss']
})
export class ComputersComponent implements OnInit {
  
  @ViewChild(RefDirective, {static: false}) refDir: RefDirective;
  computers: IComputer[];
  addComputerForm: FormGroup = new FormGroup({
    id: new FormControl(0),
    name: new FormControl('', Validators.required),
    ssdAmount: new FormControl('', Validators.required),
    ramAmount: new FormControl('', Validators.required),
    osType: new FormControl(<number>OsType.Linux),
  });

  constructor(private computerService: ComputerService,
              private resolver: ComponentFactoryResolver) { }

  ngOnInit(): void {
    this.getAllComputers();
  }
  getAllComputers() {
      this.computerService.getAll().subscribe((value: IComputer[]) => {
        this.computers = value;
      });
  }

  showEditForm(computer: IComputer) {
    const formFactory = this.resolver.resolveComponentFactory(ComputerEditDialogComponent);
    const instance = this.refDir.containerRef.createComponent(formFactory).instance;
    instance.computer = {
      id: computer.id,
      name: computer.name,
      ramAmount: computer.ramAmount,
      ssdAmount: computer.ssdAmount,
      osType: computer.osType,
      cpu: computer.cpu
    };
    instance.onCancel.subscribe(() => {this.refDir.containerRef.clear(); this.ngOnInit(); });
  }

  addNewComputer() {
    const formFactory = this.resolver.resolveComponentFactory(ComputerAddDialogComponent);
    const instance = this.refDir.containerRef.createComponent(formFactory).instance;
    instance.onCancel.subscribe(() => {this.refDir.containerRef.clear(); this.ngOnInit(); });
  }
  deleteComputer(id: number) {
    this.computerService.delete(id).subscribe(() => {
      this.ngOnInit();
    });
  }
}

