import { CpuService } from './../../../core/services/cpu.service';
import { IComputer } from './../../../core/models/computer';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { OsType } from 'src/app/core/models/OsType.enum';
import { ComputerService } from 'src/app/core/services/computer.service';
import { ICpu } from 'src/app/core/models/cpu';

@Component({
  selector: 'app-computer-add-dialog',
  templateUrl: './computer-add-dialog.component.html',
  styleUrls: ['./computer-add-dialog.component.scss']
})
export class ComputerAddDialogComponent implements OnInit {

  @Output() onCancel: EventEmitter<void> = new EventEmitter<void>();
  addComputerForm: FormGroup;
  cpus: ICpu[];

  constructor(private computerService: ComputerService, private cpuService: CpuService) { }

  ngOnInit(): void {
    this.addComputerForm = new FormGroup({
      name: new FormControl('', Validators.required),
      ssdAmount: new FormControl(0, Validators.required),
      ramAmount: new FormControl(0, Validators.required),
      osType: new FormControl(<number>OsType.Linux),
      cpu: new FormControl(null)
    });
    this.cpuService.getAll().subscribe((data: ICpu[]) => {
      this.cpus = data
    });
  }

  onSubmit() {
    const newComputer: IComputer = {
      name: this.addComputerForm.get('name').value,
      ramAmount: this.addComputerForm.get('ramAmount').value,
      ssdAmount: this.addComputerForm.get('ssdAmount').value,
      osType: this.addComputerForm.get('osType').value,
      cpu: this.cpus.find(x=>x.id == this.addComputerForm.get('cpu').value),
    };
    newComputer.cpu.id = 0;
    this.computerService.post(newComputer).subscribe(() => {
      this.onCancel.emit();
    });
  }

  cancel() {
    this.onCancel.emit();
  }
}
