import { AdminService } from './../../services/admin/admin.service';
import { FormGroup, FormControl } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { CreateTitle } from 'src/app/models/createTitle';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  createTitleForm: FormGroup;

  constructor(private readonly adminService: AdminService) { }

  ngOnInit() {
    this.createTitleForm = new FormGroup({
      name: new FormControl(''),
      description: new FormControl(''),
      ageRestriction: new FormControl('')
    });
  }

  onSubmit() {
    console.log('here');
    this.adminService.createTitle(this.createTitleForm.value).subscribe();
  }
}
