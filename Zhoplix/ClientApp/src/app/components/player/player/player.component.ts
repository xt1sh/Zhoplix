import { Component, OnInit } from '@angular/core';
import { VgAPI } from 'videogular2/compiled/core';
import { PlayerMedia } from 'src/app/models/player-media';

@Component({
  selector: 'app-player',
  templateUrl: './player.component.html',
  styleUrls: ['./player.component.scss']
})
export class PlayerComponent implements OnInit {
  video: PlayerMedia;

  constructor() { }

  ngOnInit() {
    this.video = {
      title: 'El Camino',
      src: 'http://localhost:5000/Videos/Uploaded/ElCamino/ElCamino.mp4',
      type: 'video/mp4'
    }
  }
}
