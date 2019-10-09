import { AdminService } from './../../services/admin/admin.service';
import { FormGroup, FormControl, FormArray } from '@angular/forms';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  createTitleForm: FormGroup;
  seasonForm: FormGroup;
  seasonsArray: FormArray;
  episodesArray: FormArray;

  constructor(private readonly adminService: AdminService) { }

  ngOnInit() {
    this.createTitleForm = new FormGroup({
      name: new FormControl(''),
      description: new FormControl(''),
      ageRestriction: new FormControl('')
    });
    // this.seasonForm = new FormGroup({
    //   seasons: new FormArray([
    //     new FormGroup({
    //       seasonName: new FormControl(null),
    //       episodes: new FormArray([
    //         new FormGroup({
    //           episodeName: new FormControl(null)
    //         })
    //       ])
    //     })
    //   ])
    // });
    this.seasonForm = this.newSeasonForm();
  }

  newEpisodeGroup() {
    return new FormGroup({
      episodeName: new FormControl(null)
    });
  }

  newEpisodeArray() {
    return new FormArray([
      this.newEpisodeGroup()
    ]);
  }

  newSeasonsArray() {
    return new FormArray([
      new FormGroup({
        seasonName: new FormControl(null),
        episodes: this.newEpisodeArray()
      })
    ]);
  }

  newSeasonGroup() {
    return new FormGroup({
      seasonName: new FormControl(null),
      episodes: this.newEpisodeArray()
    });
  }

  newSeasonForm() {
    return new FormGroup({
      seasons: this.newSeasonsArray()
    });
  }

  createSeason() {
    (this.seasonForm.controls.seasons as FormArray).push(this.newSeasonGroup());
  }

  deleteSeason(index: number) {
    (this.seasonForm.controls.seasons as FormArray).removeAt(index);
  }

  createEpisode(season: number) {
    this.getEpisodesArray(season).push(this.newEpisodeGroup());
  }

  deleteEpisode(season: number, episode: number) {
    this.getEpisodesArray(season).removeAt(episode);
  }

  getEpisodesArray(season: number) {
    return ((this.seasonForm.controls.seasons as FormArray).at(season) as FormGroup).controls['episodes'] as FormArray;
  }

  onSubmit() {
    this.adminService.createTitle(this.createTitleForm.value).subscribe();
  }
}
