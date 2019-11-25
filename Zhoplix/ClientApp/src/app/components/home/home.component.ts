import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  tvSrc = 'Images/Home/tv.png';
  mobileSrc = 'Images/Home/mobile.png';
  mobile2Src = 'Images/Home/mobile2.png';
  downloadSrc = 'Images/Home/download-icon.gif';
  devicesSrc = 'Images/Home/devices.png';
  tvVideo = 'Videos/Home/tvVideo.mp4';
  devicesVideo = 'Videos/Home/devices.mp4';

  closedStates: Array<boolean>;

  constructor(private readonly router: Router) {}

  ngOnInit() {
    let video = document.getElementById('video') as HTMLVideoElement;
    video.muted = true;
    video = document.getElementById('video2') as HTMLVideoElement;
    video.muted = true;
    this.closedStates = new Array<boolean>();
    for(let i = 0; i < 6; i++) {
      this.closedStates.push(false);
    }
    this.closeAll();
  }

  navigateSignUp() {
    this.router.navigate(['signup']);
  }

  toggle(id) {
    if(this.closedStates[+id]) {
      this.closeAllExcept(+id);
    } else {
      this.closeAll();
    }
  }

  closeAll() {
    this.closedStates.forEach((x, index) => {
      this.closedStates[index] = true;
    });
  }

  closeAllExcept(index: number) {
    this.closeAll();
    this.closedStates[index] = false;
  }
}
