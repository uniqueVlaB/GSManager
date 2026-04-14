import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SettingPicker } from './setting-picker';

describe('SettingPicker', () => {
  let component: SettingPicker;
  let fixture: ComponentFixture<SettingPicker>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SettingPicker]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SettingPicker);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
