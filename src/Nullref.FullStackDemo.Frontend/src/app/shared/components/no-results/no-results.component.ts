import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-no-results',
  templateUrl: './no-results.component.html',
  styleUrls: ['./no-results.component.scss'],
})
export class NoResultsComponent implements OnInit {
  @Input() errorMessage = 'There are no items';
  @Input() subtitle?: string;
  @Input() buttonText?: string;
  @Output() createButtonAction?= new EventEmitter<string[]>();

  constructor() { }

  ngOnInit(): void { }

  createButtonClick(): void {
    this.createButtonAction?.emit();
  }

}
