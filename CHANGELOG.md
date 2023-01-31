# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.0.4] - 2023-01-31
### Changed
- Fixed NullReferenceException caused by empty TransitionGroups or null entries.

## [1.0.3] - 2023-01-27
### Changed
- 'Transition_CanvasGroupAlpha' now has its 'start' and 'end' values clamped between 0.0 and 1.0.
- 'TransitionGroup' now displays the total duration of the group including its 'delay' in its 'duration' field in the inspector.
- Removed Text Mesh Pro Package as a dependency.

### Added
- Added Documentation.

## [1.0.2] - 2023-01-22
### Changed
- Updated Promises Package dependency from 2.1.0 to 2.1.1.
- Removed 'Everything' flag from code because it was causing serialization problems.

### Fixed
- Fixed issue where Transition could end immediately upon starting for the first time if GameObject was inactive prior to calling 'Play'.

## [1.0.1] - 2023-01-21
### Changed
- Cleaned up the code.
- Methods 'Initialize', 'OnTransitionStart', OnTransitionUpdate', and 'OnTransitionEnd' are now virtual instead of abstract.
- Updated Promises Package dependency from 2.0.0 to 2.1.0.
- Updated minimum Unity version from 2021.2 to 2021.3.

## [1.0.0] - 2022-03-14
### Changed
- Updated Promises Package dependency from 1.1.0 to 2.0.0.

## [0.9.0-preview.1] - 2022-03-09
This is the first release of *Vulpes Transitions* as a Package.