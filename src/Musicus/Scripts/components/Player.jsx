import React, { PropTypes } from 'react'
import { connect } from 'react-redux'
import Ink from 'react-ink';
import { next, play, pause } from '../actions/generalActions.jsx'

class Player extends React.Component {
  constructor(props) {
    super(props);
    this.clickPlay = this.clickPlay.bind(this);
    this.clickNext = this.clickNext.bind(this);
  }

	clickPlay() {
    if (this.props.isplaying) {
			this.props.pause(this.props.currentTrack);
    } else {
      this.props.play(this.props.currentTrack);
    }
  }

  clickNext() {
    this.props.next();
  }

  render() {
    var btnStyle = {
      margin: "10px"
    }

    var icon = 'icon step play';
    if (this.props.isplaying) {
      icon = 'icon step pause';
    }
    return (
      <div className="controls-buttons">
        <div onTouchTap={this.clickPlay} className="button">
          <i className={icon} style={btnStyle}></i>
          <Ink />
        </div>
        <div onTouchTap={this.clickNext} className="button">
          <i className='icon step forward' style={btnStyle}></i>
          <Ink />
        </div>
      </div>

    )
  }
}

Player.propTypes = {

}

function mapStateToProps(state) {
  return {
		isplaying: state.musicusState.isplaying,
		currentTrack: state.musicusState.currentTrack
  }
}

export default connect(mapStateToProps, { next, play, pause })(Player)
