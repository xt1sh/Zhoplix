import { EpisodesService } from './../../../services/CRUD/episodes.service';
import { Component, OnInit, Input, ElementRef } from '@angular/core';
import { VgAPI, VgStates } from 'videogular2/compiled/core';
import { EpisodeForPlayerModel } from 'src/app/models/episode/episode-for-player-model';
import { ShadeAnimation } from 'src/app/animations/shade-animation';
import { fromEvent } from 'rxjs';

@Component({
  selector: 'app-player',
  templateUrl: './player.component.html',
  styleUrls: ['./player.component.scss'],
  animations: [
    ShadeAnimation
  ]
})
export class PlayerComponent implements OnInit {

  @Input() episodeId: number;

  currentVideoSrc: string;
  api: VgAPI;
  episode: EpisodeForPlayerModel;
  isSingleClick: boolean;
  thumbSrc: string;
  thumbLocation: string;
  scrubWidth: number;
  isMouseOverScrub: boolean;
  thumbnail: HTMLElement;
  currentState: string;
  mouseOnControls: boolean;
  mouseOverPlayer: boolean;
  timeout: any;

  constructor(private readonly episodeService: EpisodesService,
    private elRef: ElementRef) { }

  ngOnInit() {
    this.mouseOverPlayer = false;
    this.mouseOnControls = false;
    this.currentState = 'visible';
    this.episodeService.getEpisodeById(this.episodeId).subscribe(result => {
      this.episode = result.body;
      this.thumbLocation = this.episode.thumbnailLocation;
      this.currentVideoSrc = this.episode.videos[0].location;
      this.api.play();
      const player = this.elRef.nativeElement.querySelector("video");
      player.load();
    });
    this.isMouseOverScrub = false;
    this.isSingleClick = true;
  }

  onPlayerReady(api: VgAPI) {
    this.thumbnail = document.getElementById('thumbnail');
    this.api = api;
    this.api.getDefaultMedia().subscriptions.ended.subscribe(() => {
      this.api.getDefaultMedia().currentTime = 0;
    });
  }

  onVideoClick() {
    this.controlsVisible();
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

  onMouseOverScrub(event) {
    this.changeThumbPosition(event.offsetX + 132);
    this.isMouseOverScrub = true;
    this.thumbSrc = this.thumbLocation + '/' + (Math.min((Math.floor((event.offsetX + 2) / (event.target.clientWidth / this.episode.thumbnailsAmount)) + 1), this.episode.thumbnailsAmount)) + '.png';
  }

  onMouseMoveScrub(event) {
    if(this.isMouseOverScrub) {
      this.changeThumbPosition(event.offsetX + 132);
      this.thumbSrc = this.thumbLocation + '/' + (Math.min((Math.floor((event.offsetX + 2) / (event.target.clientWidth / this.episode.thumbnailsAmount)) + 1), this.episode.thumbnailsAmount)) + '.png';
    }
  }

  onMouseLeaveScrub() {
    this.isMouseOverScrub = false;
  }

  onMouseMove() {
    this.controlsVisible();
    if(!this.mouseOnControls) {
      clearTimeout(this.timeout);
      this.timeout = setTimeout(() => {
        this.controlsInvisible();
      }, 2000);
    }
  }

  onMouseEnter() {
    this.mouseOverPlayer = true;
    this.controlsVisible();
  }

  onMouseLeave() {
    this.mouseOverPlayer = false;
    this.controlsInvisible();
  }

  changeThumbPosition(x: number) {
    this.thumbnail.style.left = `${x}px`;
  }

  controlsVisible() {
    document.body.style.cursor = 'auto';
    this.currentState = 'visible';
  }

  controlsInvisible() {
    if(!this.mouseOnControls) {
      this.currentState = 'invisible';
      if(this.mouseOverPlayer) {
        document.body.style.cursor = 'none';
      }
    }
  }
}
