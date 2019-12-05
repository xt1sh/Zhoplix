import { Component, OnInit, ViewChild } from '@angular/core';
import { CarouselComponent } from 'angular-bootstrap-md';
import { MainService } from 'src/app/services/browse/main.service';
import { Title } from 'src/app/models/title';
import { registerLocaleData } from '@angular/common';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit {

  @ViewChild(CarouselComponent, {static: true}) item: CarouselComponent;
  myListArray: Array<Title>;
  slidesCount: number;
  loading: boolean;
  constructor(private readonly mainService: MainService) { }

  ngOnInit() {
    this.loading = true;
    this.myListArray = new Array<Title>();
    const observer = {
      next: (response) =>{
        response.body.forEach(element => {
          this.myListArray.push({ Id:element['id'], ImagePath: `Images\\Uploaded\\${element['imageId']}\\${element['imageId']}.png`})
        });
        this.loading = false;
        console.log(this.myListArray);
      }, error: null, complete: () => {
        
      }
    };
    this.mainService.getMyListSize().subscribe(response =>
      {
        this.slidesCount = Math.floor(response.body.length / 6 + 1);
        console.log(this.slidesCount);
      });
      this.mainService.getMyListPage(1, 6).subscribe(observer);
  }
  
}
