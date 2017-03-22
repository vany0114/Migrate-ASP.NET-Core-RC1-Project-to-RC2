module.exports = function (grunt) {
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-contrib-less');

    grunt.initConfig({
        concat: {
            dist: {
                files: {
                    'wwwroot/js/libs.js': ['Scripts/Libs/*.js']
                }
            }
        },
        uglify: {
            my_target: {
                files: {
                    'wwwroot/js/app.js': ['Scripts/ParkingLot/module.js', 'Scripts/ParkingLot/**/*.js'],
                    'wwwroot/js/libs.js': ['wwwroot/js/libs.js']
                }
            },
            options: {
                sourceMap: true,
                sourceMapIncludeSources: true
            }
        },
        cssmin: {
            target: {
                files: [{
                    expand: true,
                    src: ['css/*.css', '!css/*.min.css'],
                    dest: 'wwwroot',
                    ext: '.min.css'
                }]
            }
        },
        less: {
            development: {
                options: {
                    paths: ["css"]
                },
                files: {
                    "wwwroot/css/site.css": "css/site.less"
                }
            }
        },
        watch: {
            scripts: {
                files: ['Scripts/**/*.js'],
                tasks: ['uglify']
            }
        }
    });
    grunt.registerTask('default', ['concat', 'uglify', 'less', 'cssmin', 'watch']);
};