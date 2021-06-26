var gulp = require('gulp');
var plugins = require('gulp-load-plugins')();
// var gutil = require('gulp-util');
// var bower = require('bower');
// var concat = require('gulp-concat');
// var sass = require('gulp-sass');
// var minifyCss = require('gulp-minify-css');
// var rename = require('gulp-rename');
// var sh = require('shelljs');
// var jshint = require('gulp-jshint');

var paths = {
  sass: ['./scss/**/*.scss']
};

gulp.task('default', ['sass']);

gulp.task('sass', function(done) {
  gulp
    .src('./scss/ionic.app.scss')
    .pipe(plugins.sass({
      errLogToConsole: true
    }))
    .pipe(gulp.dest('./www/css/'))
    .pipe(plugins.minifyCss({
      keepSpecialComments: 0
    }))
    .pipe(plugins.rename({ extname: '.min.css' }))
    .pipe(gulp.dest('./www/css/'))
    .on('end', done);
});

gulp.task('serve', function(done) {
  plugins.sh.exec('ionic serve', done);
});

gulp.task('watch', function() {
  gulp
    .watch(paths.sass, ['sass']);
});

gulp.task('jshint', function() {
  return gulp.src('./www/js/**/*.js')
    .pipe(plugins.jshint())
    .pipe(plugins.jshint.reporter('jshint-stylish'));
});

gulp.task('install', ['git-check'], function() {
  return plugins.bower.commands.install()
    .on('log', function(data) {
      plugins.gutil.log('bower', plugins.gutil.colors.cyan(data.id), data.message);
    });
});

gulp.task('git-check', function(done) {
  if (!plugins.sh.which('git')) {
    console.log(
      '  ' + plugins.gutil.colors.red('Git is not installed.'),
      '\n  Git, the version control system, is required to download Ionic.',
      '\n  Download git here:', plugins.gutil.colors.cyan('http://git-scm.com/downloads') + '.',
      '\n  Once git is installed, run \'' + plugins.gutil.colors.cyan('gulp install') + '\' again.'
    );
    process.exit(1);
  }
  done();
});