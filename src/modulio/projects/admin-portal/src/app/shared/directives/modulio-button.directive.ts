import { Directive, ElementRef, Input, Renderer2, OnInit } from '@angular/core';

@Directive({
  selector: '[modulioButton]'
})
export class ModulioButtonDirective implements OnInit {
  @Input() variant: 'primary' | 'secondary' | 'danger' | 'success' = 'primary';
  @Input() size: 'small' | 'medium' | 'large' = 'medium';
  @Input() disabled: boolean = false;

  constructor(
    private el: ElementRef,
    private renderer: Renderer2
  ) { }

  ngOnInit() {
    this.applyStyles();
  }

  private applyStyles() {
    const button = this.el.nativeElement;

    // Base styles
    this.renderer.addClass(button, 'modulio-btn');
    this.renderer.addClass(button, `modulio-btn--${this.variant}`);
    this.renderer.addClass(button, `modulio-btn--${this.size}`);

    // Disabled state
    if (this.disabled) {
      this.renderer.addClass(button, 'modulio-btn--disabled');
      this.renderer.setAttribute(button, 'disabled', 'true');
    }
  }
}
