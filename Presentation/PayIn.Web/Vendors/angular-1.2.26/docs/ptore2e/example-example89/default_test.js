describe("", function() {
  var rootEl;
  beforeEach(function() {
    rootEl = browser.rootEl;
    browser.get("examples/example-example89/index.html");
  });
  
  it('should calculate expression in binding', function() {
    expect(element(by.binding('1+2')).getText()).toEqual('1+2=3');
  });
});