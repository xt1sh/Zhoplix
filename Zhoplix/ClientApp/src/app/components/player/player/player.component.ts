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
  clicks: number;
  timer: any;

  constructor() { }

  ngOnInit() {
    this.clicks = 0;
    this.video = {
      title: "El Camino",
      src: "http://localhost:5000/Videos/Uploaded/The.Hateful.Eight.mp4",
      type: "video/mp4"
    };
  }

  onPlayerReady(api: VgAPI) {
    this.api = api;
    this.api.getDefaultMedia().subscriptions.ended.subscribe(() => {
      // Set the video to the beginning
      this.api.getDefaultMedia().currentTime = 0;
    });
    this.api.play();
  }

  onVideoClick() {
    this.clicks++;
    this.timer = setTimeout(() => {
      if(this.clicks < 2 && this.clicks !== 0) {
        this.clicks = 0;
        this.playPauseClick();
      } else if(this.clicks !== 0) {
        this.clicks = 0;
        this.onVideoDclick();
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
    this.api.fsAPI.toggleFullscreen();
  }
}
