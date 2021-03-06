describe("", function() {
  var rootEl;
  beforeEach(function() {
    rootEl = browser.rootEl;
    browser.get("examples/example-example91/index-jquery.html");
  });
  
  it('should calculate expression in binding', function() {
    if (browser.params.browser == 'safari') {
      // Safari can't handle dialogs.
      return;
    }
    element(by.css('[ng-click="greet()"]')).click();

    var alertDialog = browser.switchTo().alert();

    expect(alertDialog.getText()).toEqual('Hello World');

    alertDialog.accept();
  });
});