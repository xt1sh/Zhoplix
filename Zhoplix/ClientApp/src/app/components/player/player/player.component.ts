import { EpisodesService } from './../../../services/CRUD/episodes.service';
import { Component, OnInit, Input, ElementRef } from '@angular/core';
import { VgAPI, VgStates } from 'videogular2/compiled/core';
import { EpisodeForPlayerModel } from 'src/app/models/episode/episode-for-player-model';

@Component({
  selector: 'app-player',
  templateUrl: './player.component.html',
  styleUrls: ['./player.component.scss']
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
  isMouseOver: boolean;
  thumbnail: HTMLElement;

  constructor(private readonly episodeService: EpisodesService,
    private elRef: ElementRef) { }

  ngOnInit() {
    this.episodeService.getEpisodeById(this.episodeId).subscribe(result => {
      this.episode = result.body;
      this.thumbLocation = this.episode.thumbnailLocation;
      this.currentVideoSrc = this.episode.videos[0].location;
      this.api.play();
      const player = this.elRef.nativeElement.querySelector("video");
      player.load();
    });
    this.isMouseOver = false;
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
    this.changeThumbPosition(event.offsetX + 132);
    this.isMouseOver = true;
    this.thumbSrc = this.thumbLocation + '/' + (Math.floor((event.offsetX + 2) / (event.target.clientWidth / 126)) + 1) + '.png';
  }

  onMouseMove(event) {
    if(this.isMouseOver) {
      this.changeThumbPosition(event.offsetX + 132);
      this.thumbSrc = this.thumbLocation + '/' + (Math.floor((event.offsetX + 2) / (event.target.clientWidth / 126)) + 1) + '.png';
    }
  }

  onMouseLeave() {
    this.isMouseOver = false;
  }

  changeThumbPosition(x: number) {
    this.thumbnail.style.left = `${x}px`;
  }
}
