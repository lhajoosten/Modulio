import { Directive, ElementRef, HostListener, Input, Renderer2 } from '@angular/core';

@Directive({
  selector: '[modulioTooltip]'
})
export class ModulioTooltipDirective {
  @Input('modulioTooltip') tooltipText: string = '';
  @Input() tooltipPosition: 'top' | 'bottom' | 'left' | 'right' = 'top';

  private tooltipElement: HTMLElement | null = null;

  constructor(
    private el: ElementRef,
    private renderer: Renderer2
  ) { }

  @HostListener('mouseenter', ['$event'])
  onMouseEnter(event: MouseEvent): void {
    if (this.tooltipText) {
      this.showTooltip();
    }
  }

  @HostListener('mouseleave', ['$event'])
  onMouseLeave(event: MouseEvent): void {
    this.hideTooltip();
  }

  private showTooltip(): void {
    if (this.tooltipElement) {
      return;
    }

    this.tooltipElement = this.renderer.createElement('div');
    this.renderer.addClass(this.tooltipElement, 'modulio-tooltip');
    this.renderer.addClass(this.tooltipElement, `tooltip-${this.tooltipPosition}`);
    this.renderer.setProperty(this.tooltipElement, 'textContent', this.tooltipText);

    this.renderer.appendChild(document.body, this.tooltipElement);
    this.positionTooltip();
  }

  private hideTooltip(): void {
    if (this.tooltipElement) {
      this.renderer.removeChild(document.body, this.tooltipElement);
      this.tooltipElement = null;
    }
  }

  private positionTooltip(): void {
    if (!this.tooltipElement) return;

    const hostRect = this.el.nativeElement.getBoundingClientRect();
    const tooltipRect = this.tooltipElement.getBoundingClientRect();

    let top: number;
    let left: number;

    switch (this.tooltipPosition) {
      case 'top':
        top = hostRect.top - tooltipRect.height - 8;
        left = hostRect.left + (hostRect.width - tooltipRect.width) / 2;
        break;
      case 'bottom':
        top = hostRect.bottom + 8;
        left = hostRect.left + (hostRect.width - tooltipRect.width) / 2;
        break;
      case 'left':
        top = hostRect.top + (hostRect.height - tooltipRect.height) / 2;
        left = hostRect.left - tooltipRect.width - 8;
        break;
      case 'right':
        top = hostRect.top + (hostRect.height - tooltipRect.height) / 2;
        left = hostRect.right + 8;
        break;
      default:
        top = hostRect.top - tooltipRect.height - 8;
        left = hostRect.left + (hostRect.width - tooltipRect.width) / 2;
    }

    this.renderer.setStyle(this.tooltipElement, 'position', 'fixed');
    this.renderer.setStyle(this.tooltipElement, 'top', `${top}px`);
    this.renderer.setStyle(this.tooltipElement, 'left', `${left}px`);
    this.renderer.setStyle(this.tooltipElement, 'z-index', '9999');
    this.renderer.setStyle(this.tooltipElement, 'background-color', '#333');
    this.renderer.setStyle(this.tooltipElement, 'color', 'white');
    this.renderer.setStyle(this.tooltipElement, 'padding', '8px 12px');
    this.renderer.setStyle(this.tooltipElement, 'border-radius', '4px');
    this.renderer.setStyle(this.tooltipElement, 'font-size', '14px');
    this.renderer.setStyle(this.tooltipElement, 'pointer-events', 'none');
    this.renderer.setStyle(this.tooltipElement, 'white-space', 'nowrap');
  }
}
