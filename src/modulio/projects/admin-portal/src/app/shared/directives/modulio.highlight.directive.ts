import { Directive, ElementRef, Input, OnChanges, Renderer2, SimpleChanges } from '@angular/core';

@Directive({
  selector: '[modulioHighlight]'
})
export class ModulioHighlightDirective implements OnChanges {
  @Input() modulioHighlight: string = '';
  @Input() highlightColor: string = 'yellow';
  @Input() caseSensitive: boolean = false;

  constructor(
    private el: ElementRef,
    private renderer: Renderer2
  ) { }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['modulioHighlight'] || changes['highlightColor'] || changes['caseSensitive']) {
      this.highlight();
    }
  }

  private highlight(): void {
    const element = this.el.nativeElement;
    const text = element.textContent || element.innerText;

    if (!this.modulioHighlight || !text) {
      element.innerHTML = text;
      return;
    }

    const searchText = this.caseSensitive ? this.modulioHighlight : this.modulioHighlight.toLowerCase();
    const targetText = this.caseSensitive ? text : text.toLowerCase();

    let highlightedText = text;

    if (targetText.includes(searchText)) {
      const regex = new RegExp(
        this.escapeRegExp(this.modulioHighlight),
        this.caseSensitive ? 'g' : 'gi'
      );

      highlightedText = text.replace(regex,
        `<span style="background-color: ${this.highlightColor}">$&</span>`
      );
    }

    element.innerHTML = highlightedText;
  }

  private escapeRegExp(string: string): string {
    return string.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
  }
}
