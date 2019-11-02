import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MediaUploadService } from 'src/app/services/media/media-upload.service';
import { AdminService } from 'src/app/services/admin/admin.service';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import * as _ from 'lodash';
import { interval, BehaviorSubject, Subject } from 'rxjs';
import { delay, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { CreateSeason } from 'src/app/models/admin/create-season';

@Component({
  selector: "app-season",
  templateUrl: "./season.component.html"
})
export class SeasonComponent implements OnInit {

  type: string;
  form: any;
  progress: number;
  message: string;
  uploading: boolean;
  titlesList: Array<TitlePageView>;
  titlesToShow: Array<TitlePageView>;
  searchObject: string;
  pageNumber: number;
  config: any;
  searchChange: Subject<string>;

  constructor(
    private readonly fb: FormBuilder,
    private readonly media: MediaUploadService,
    private readonly admin: AdminService,
    private readonly snack: MatSnackBar,
    private readonly router: Router
  ) {}

  ngOnInit() {
    this.searchChange = new Subject<string>();
    this.titlesList = new Array<TitlePageView>();
    this.titlesToShow = new Array<TitlePageView>();
    this.pageNumber = 1;
    this.uploading = false;
    this.config = {
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
    this.getTitlePage();
    this.form = this.fb.group({
      name: '',
      description: '',
      title: [],
      imageId: ''
    });
    this.searchChange.pipe(
      debounceTime(500),
      distinctUntilChanged())
    .subscribe(value => this.search(value));
  }

  getTitlePage() {
    this.admin.getTitlesPage(this.pageNumber, 5).subscribe(value => {
      this.titlesList = this.titlesList.concat(value.body);
      this.titlesList = _.uniqWith(this.titlesList, _.isEqual);
      this.pageNumber++;
    });
  }

  search(value) {
    if(value) {
      this.admin.findTitles(value).subscribe(result => {
        this.titlesList = this.titlesList.concat(result.body);
        this.titlesList = _.uniqWith(this.titlesList, _.isEqual);
        this.titlesToShow = result.body;
      })
    }
  }

  onSubmit() {
    let season: CreateSeason = this.form.value;
    season.titleId = +this.form.controls['title'].value.id;
    this.admin.createSeason(season).subscribe(() => {
      this.snack.open(`Season "${season.name}" was successfully created`,
        'OK', {duration: 3000, panelClass: ['snack-success']});
      this.router.navigateByUrl('/admin/create');
    })
  }
}

export interface TitlePageView {
  id: number;
  name: string;
}
