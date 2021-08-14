import { Component, Input, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { faUser, faComment, faHeart } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() member : Member;

  userIcon = faUser;
  heartIcon = faHeart;
  messageIcon = faComment;
  constructor() { }

  ngOnInit(): void {
  }

}
