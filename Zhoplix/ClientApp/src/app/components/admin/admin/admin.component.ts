import { AdminService } from './../../../services/admin/admin.service';
import { FormGroup, FormControl, FormArray, FormBuilder } from '@angular/forms';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  constructor(private readonly adminService: AdminService,
              private readonly fb: FormBuilder) { }

  ngOnInit() {}
}
