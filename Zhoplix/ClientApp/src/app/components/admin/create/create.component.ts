import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { MediaUploadService } from '../../../services/media/media-upload.service';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.scss']
})
export class CreateComponent implements OnInit {

  type: string;
  form: any;
  progress: number;
  hasImageInInput: boolean;

  constructor(private readonly activatedRoute: ActivatedRoute,
              private readonly fb: FormBuilder,
              private readonly media: MediaUploadService) { }

  ngOnInit() {
    this.hasImageInInput = false;
    this.activatedRoute.params.subscribe(params => {
      this.type = params['id'].toLowerCase();
    });
    this.createForm();
    // this.media.getProgress().subscribe(value => {
    //   this.progress = value;
    // })
  }

  createForm() {
    if(this.type === 'title') {
      this.form = this.fb.group({
        name: '',
        description: '',
        ageRestriction: '0',
        image: null
      })
    }
  }

  uploadPhoto(files) {
    this.media.uploadVideo(files[0]);
  }
}
