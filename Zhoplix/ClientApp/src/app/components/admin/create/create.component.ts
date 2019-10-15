import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.scss']
})
export class CreateComponent implements OnInit {

  type: string;
  form: any;

  constructor(private readonly activatedRoute: ActivatedRoute,
              private readonly fb: FormBuilder) { }

  ngOnInit() {
    this.activatedRoute.params.subscribe(params => {
      this.type = params['id'].toLowerCase();
    })
  }

  createForm() {
    if(this.type === 'title') {
      this.form = this.fb.group({
        name: '',
        description: '',
        ageRestriction: '0'
      })
    }
  }
}
