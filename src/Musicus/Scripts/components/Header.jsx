import React, { PropTypes } from 'react'
import { connect } from 'react-redux'
import Ink from 'react-ink';
import { next, play, pause, setVolume } from '../actions/generalActions.jsx'
import Player from './Player.jsx'
import VolumeBar from './VolumeBar.jsx'
import moment from 'moment'

class Header extends React.Component {

  render() {
    let currentTrackArtist = '';
    let currentTrackTrack = '';
    let progressbarValue = { width: '0%' };

    let currentTrackTime = '0:00';
    let maxTrackTime = '0:00';

    if (this.props.trackInfo) {
      currentTrackArtist = `${this.props.trackInfo.artist}`;
      currentTrackTrack = `${this.props.trackInfo.track}`;

      var maxTime = moment.duration(this.props.trackInfo.length * 1000);
      maxTrackTime = `${maxTime.minutes()}:${maxTime.seconds().pad()}`;

      var currentTime =  moment.duration(this.props.trackInfo.currentPosition * 1000);
      currentTrackTime = `${currentTime.minutes()}:${currentTime.seconds().pad()}`;

      const trackDone = (100.0 / this.props.trackInfo.length) * this.props.trackInfo.currentPosition;
      progressbarValue = {
        width: `${trackDone}%`
      }
    }

    return (
      <div className="page-header">
        <div className="page-header-album-art">
          <img className="responsive-image" src={this.props.trackInfo.albumArtwork} />
        </div>
        <div className="page-header-controls">
          <div className="controls-song">
            <div className="controls-song-track">
              {currentTrackTrack} <br />
            </div>
            <div className="controls-song-artist">
              {currentTrackArtist} <br />
            </div>
          </div>
          <div className="controls-controls">
            <Player />
            <VolumeBar />
            <div className="controls-progressbar">
              <div className="progressbar-tracktime">
                {currentTrackTime}
              </div>
              <div className="progressbar-container">
                <div className="progressbar-value" style={progressbarValue}><div>
                </div>
                </div>
              </div>
              <div className="progressbar-tracktime">
                {maxTrackTime}
              </div>
            </div>
          </div>
        </div>
      </div>
    )
  }
}

Header.propTypes = {

}

function mapStateToProps(state) {
  return {
		trackInfo: state.musicusState.currentTrack
  }
}

export default connect(mapStateToProps, { next, play, pause, setVolume })(Header)
