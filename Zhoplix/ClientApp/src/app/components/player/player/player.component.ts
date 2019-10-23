import { Component, OnInit } from '@angular/core';
import { VgAPI, VgStates } from 'videogular2/compiled/core';
import { PlayerMedia } from 'src/app/models/player-media';
import { Observable, BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-player',
  templateUrl: './player.component.html',
  styleUrls: ['./player.component.scss']
})
export class PlayerComponent implements OnInit {

  api: VgAPI
  video: PlayerMedia;
  isSingleClick: boolean;
  thumbSrc: string;
  thumbs = 'http://localhost:5000/Videos/Uploaded/ElCamino/Thumbnails';
  scrubWidth: number;
  isMouseOver: boolean;
  thumbnail: HTMLElement;

  constructor() { }

  ngOnInit() {
    this.isMouseOver = false;
    this.isSingleClick = true;
    this.video = {
      title: "El Camino",
      src: "http://localhost:5000/Videos/Uploaded/ElCamino/ElCamino.mp4",
      type: "video/mp4"
    };
  }

  onPlayerReady(api: VgAPI) {
    this.thumbnail = document.getElementById('thumbnail');
    console.log(this.thumbnail);
    this.api = api;
    this.api.getDefaultMedia().subscriptions.ended.subscribe(() => {
      this.api.getDefaultMedia().currentTime = 0;
    });
    // this.api.play(); доебало
  }

  onVideoClick() {
    this.isSingleClick = true;
    setTimeout(() => {
      if(this.isSingleClick) {
        this.playPauseClick();
      }
    }, 250);
  }

  playPauseClick() {
    switch (this.api.state) {
      case VgStates.VG_LOADING: {
        this.api.pause();
        break;
      }
      case VgStates.VG_PAUSED:
      case VgStates.VG_ENDED: {
        this.api.play();
        break;
      }
      case VgStates.VG_PLAYING: {
        this.api.pause();
        break;
      }
    }
  }

  onVideoDclick() {
    this.isSingleClick = false;
    this.api.fsAPI.toggleFullscreen();
  }

  onMouseOver(event) {
    this.changeThumbPosition(event.clientX - 60);
    this.isMouseOver = true;
    this.thumbSrc = this.thumbs + '/' + (Math.floor((event.offsetX + 2) / (event.target.clientWidth / 48)) + 1) + '.png';
  }

  onMouseMove(event) {
    if(this.isMouseOver) {
      this.changeThumbPosition(event.clientX - 60);
      this.thumbSrc = this.thumbs + '/' + (Math.floor((event.offsetX + 2) / (event.target.clientWidth / 48)) + 1) + '.png';
    }
  }

  onMouseLeave() {
    this.isMouseOver = false;
  }

  changeThumbPosition(x: number) {
    this.thumbnail.style.left = `${x}px`;
  }
}
