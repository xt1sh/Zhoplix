import { Component, OnInit } from '@angular/core';
import { IdName } from 'src/app/models/admin/id-name';
import { Subject } from 'rxjs';
import { FormBuilder } from '@angular/forms';
import { AdminService } from 'src/app/services/admin/admin.service';
import { MediaUploadService } from 'src/app/services/media/media-upload.service';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import { distinctUntilChanged, debounceTime } from 'rxjs/operators';
import { CreateSeason } from 'src/app/models/admin/create-season';
import * as _ from 'lodash';
import { CreateTitle } from 'src/app/models/admin/create-title';
import { CreateEpisode } from 'src/app/models/admin/create-episode';

@Component({
  selector: 'app-episode',
  templateUrl: './episode.component.html',
})
export class EpisodeComponent implements OnInit {

  form: any;
  message: string;
  uploading: boolean;
  titlesList: Array<IdName>;
  seasonsList: Array<IdName>;
  titlesToShow: Array<IdName>;
  pageNumber: number;
  titleConfig: any;
  seasonConfig: any;
  searchChange: Subject<string>;
  progress: number;

  constructor(
    private readonly fb: FormBuilder,
    private readonly media: MediaUploadService,
    private readonly admin: AdminService,
    private readonly snack: MatSnackBar,
    private readonly router: Router
  ) {}

  ngOnInit() {
    this.searchChange = new Subject<string>();
    this.titlesList = new Array<IdName>();
    this.titlesToShow = new Array<IdName>();
    this.pageNumber = 1;
    this.uploading = false;
    this.titleConfig = {
      displayKey: 'name',
      search: true,
      height: 'auto',
      placeholder:'Select',
      limitTo: 50,
      moreText: 'more',
      noResultsFound: 'No results found!',
      searchPlaceholder:'Choose title',
      searchOnKey: 'name',
    }
    this.seasonConfig = Object.create(this.titleConfig);
    this.seasonConfig.search = false;
    this.seasonConfig.searchPlaceholder = 'Choose season';
    this.getTitlePage();
    this.form = this.fb.group({
      name: '',
      description: '',
      title: [],
      season: [],
      videoIds: []
    });
    this.searchChange.pipe(
      debounceTime(500),
      distinctUntilChanged())
    .subscribe(value => this.searchTitle(value));
  }

  getTitlePage() {
    this.admin.getTitlesPage(this.pageNumber, 5).subscribe(value => {
      this.titlesList = this.titlesList.concat(value.body);
      this.titlesList = _.uniqWith(this.titlesList, _.isEqual);
      this.pageNumber++;
    });
  }

  searchTitle(titleName) {
    if(titleName) {
      this.admin.findTitles(titleName).subscribe(result => {
        this.titlesList = this.titlesList.concat(result.body);
        this.titlesList = _.uniqWith(this.titlesList, _.isEqual);
        this.titlesToShow = result.body;
      })
    }
  }

  uploadVideo(files) {
    this.uploading = true;
    this.media.uploadVideo(files[0]);
    this.media.getProgress().subscribe(value => {

      if (value < 99) {
        this.progress = value;
      } else {
        this.progress = 99;
      }
    });
    this.media.getBody().subscribe(value => {
      if (value) {
        this.form.controls['videoIds'].setValue(value);
        this.progress = 100;
        this.uploading = false;
      }
    });
  }

  onTitleChange(event) {
    this.seasonsList = [];
    if (event) {
      this.admin.getAllSeasonOfTitle(event.value.id).subscribe(result => {
        this.seasonsList = result.body;
      }, () => {
        this.seasonsList = [];
      })
    }
  }

  onSubmit() {
    let episode: CreateEpisode = this.form.value;
    episode.seasonId = +this.form.controls['season'].value.id;
    console.log(episode);
    this.admin.createEpisode(episode).subscribe(() => {
      this.snack.open(`Episode "${episode.name}" was successfully created`,
        'OK', {duration: 3000, panelClass: ['snack-success']});
      this.router.navigateByUrl('/admin/create');
    })
  }

}
