import { Component, OnInit, ViewChild } from '@angular/core';
import { CarouselComponent } from 'angular-bootstrap-md';
import { MainService } from 'src/app/services/browse/main.service';
import { Titles } from 'src/app/models/titles';
import { registerLocaleData } from '@angular/common';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit {

  @ViewChild(CarouselComponent, {static: true}) item: CarouselComponent;
  myListArray: Array<Titles>;

  constructor(private readonly mainService: MainService) { }

  ngOnInit() {
    console.log(this.item);
    this.myListArray = new Array<Titles>();
    this.mainService.getMyListPage(1, 2).subscribe(response => {
      response.body.forEach(element => {
        this.myListArray.push({ Id:element['id'], ImagePath: `Images\\Uploaded\\${element['imageId']}\\${element['imageId']}.png`})
      });
    })
  }

  getMyListPage() {
    this.mainService.getMyListPage(1, 6);
  }
}
