describe("", function() {
  var rootEl;
  beforeEach(function() {
    rootEl = browser.rootEl;
    browser.get("examples/example-example59/index.html");
  });
  
it('should interpolate binding with custom symbols', function() {
  expect(element(by.binding('demo.label')).getText()).toBe('This binding is brought you by // interpolation symbols.');
});
});