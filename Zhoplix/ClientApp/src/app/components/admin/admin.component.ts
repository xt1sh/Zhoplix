import { Component, OnInit } from '@angular/core';
import { CreateTitle } from 'src/app/models/createTitle';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  title: CreateTitle;

  constructor() { }

  ngOnInit() {
    this.title = new CreateTitle();
  }
}
