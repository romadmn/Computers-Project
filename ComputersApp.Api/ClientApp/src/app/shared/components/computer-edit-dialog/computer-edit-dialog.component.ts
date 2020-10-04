import { IComputer } from './../../../core/models/computer';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { OsType } from 'src/app/core/models/OsType.enum';
import { ComputerService } from 'src/app/core/services/computer.service';
import { CpuService } from 'src/app/core/services/cpu.service';
import { ICpu } from 'src/app/core/models/cpu';

@Component({
  selector: 'app-computer-edit-dialog',
  templateUrl: './computer-edit-dialog.component.html',
  styleUrls: ['./computer-edit-dialog.component.scss']
})
export class ComputerEditDialogComponent implements OnInit {

  @Output() onCancel: EventEmitter<void> = new EventEmitter<void>();
  @Input() computer: IComputer;
  editComputerForm: FormGroup;
  cpus: ICpu[];
  submitted = false;
  loading = false;

  constructor(private computerService: ComputerService, private cpuService: CpuService ) { }

  ngOnInit(): void {
    this.editComputerForm = new FormGroup({
      name: new FormControl(this.computer.name, [Validators.required, Validators.maxLength(50)]),
      ssdAmount: new FormControl(this.computer.ssdAmount, Validators.required),
      ramAmount: new FormControl(this.computer.ramAmount, Validators.required),
      osType: new FormControl(<number>this.computer.osType),
      cpu: new FormControl(this.computer.cpu.id)
    });
    this.cpuService.getAll().subscribe((data: ICpu[]) => {
      this.cpus = data;
    });
  }
  get form() { return this.editComputerForm.controls; }

  onSubmit() {
    this.submitted = true;

      if (this.editComputerForm.invalid) {
          return;
      }

      this.loading = true;
    const newComputer: IComputer = {
      id: this.computer.id,
      name: this.editComputerForm.get('name').value,
      ramAmount: this.editComputerForm.get('ramAmount').value,
      ssdAmount: this.editComputerForm.get('ssdAmount').value,
      osType: this.editComputerForm.get('osType').value,
      cpuId: this.editComputerForm.get('cpu').value
    };
    this.computerService.put(newComputer).subscribe(() => {
      this.onCancel.emit();
    });
  }

  cancel() {
    this.onCancel.emit();
  }
}
