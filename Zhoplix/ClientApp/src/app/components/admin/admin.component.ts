import { Component, OnInit } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  title: CreateTitle;

  constructor() { }

  ngOnInit() {
    this.title = {
      name: '',
      description: '',
      ageRestriction: 0,
      seasons: [{
          name: '',
          episodes: [{
            name: '',
            video: ''
          }]
        }
      ]
    }
  }

  onClick() {
    console.log(this.title.name);
  }
}

export interface CreateTitle {
  name: string;
  description: string;
  ageRestriction: number;
  seasons: CreateSeason[];
}

export interface CreateSeason {
  name: string;
  episodes: CreateEpisode[];
}

export interface CreateEpisode {
  name: string;
  video: any;
}
