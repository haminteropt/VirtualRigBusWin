import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PageNotFoundComponentComponent } from './page-not-found.component';

describe('PageNotFoundComponentComponent', () => {
  let component: PageNotFoundComponentComponent;
  let fixture: ComponentFixture<PageNotFoundComponentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PageNotFoundComponentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PageNotFoundComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
