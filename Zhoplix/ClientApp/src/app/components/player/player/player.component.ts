import { Component, OnInit } from '@angular/core';
import { VgAPI, VgStates } from 'videogular2/compiled/core';
import { PlayerMedia } from 'src/app/models/player-media';

@Component({
  selector: 'app-player',
  templateUrl: './player.component.html',
  styleUrls: ['./player.component.scss']
})
export class PlayerComponent implements OnInit {

  api: VgAPI
  video: PlayerMedia;
  isSingleClick: boolean;

  constructor() { }

  ngOnInit() {
    this.isSingleClick = true;
    this.video = {
      title: "El Camino",
      src: "http://localhost:5000/Videos/Uploaded/ElCamino/ElCamino.mp4",
      type: "video/mp4"
    };
  }

  onPlayerReady(api: VgAPI) {
    this.api = api;
    this.api.getDefaultMedia().subscriptions.ended.subscribe(() => {
      this.api.getDefaultMedia().currentTime = 0;
    });
    this.api.play();
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
}
