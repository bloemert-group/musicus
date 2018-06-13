import React, { PropTypes } from 'react'
import { connect } from 'react-redux'
import Ink from 'react-ink';
import { setVolume } from '../actions/generalActions.jsx'

class VolumeBar extends React.Component {
	constructor(props) {
		super(props);

		this.state = { currentVolume: props.volume };

		this.setVolume = this.setVolume.bind(this);
		this.onVolumeChange = this.onVolumeChange.bind(this);
	}

	setVolume(event) {
		this.props.setVolume(this.props.trackSource, event.target.value);
	}

	onVolumeChange(event) {
		this.setState({ currentVolume: event.target.value });
	}

	componentWillReceiveProps(props) {
		if (props.volume !== this.state.volume) {
			this.setState({ currentVolume: props.volume });
		}
	}

	render() {

		return (
			<div className="controls-volume">
				<input type="range" id="controls-volumebar" min="0" step="1" max="100" value={this.state.currentVolume} onMouseUp={this.setVolume} onChange={this.onVolumeChange} />
			</div>
		)
	}
}

function mapStateToProps(state) {
	return {
		volume: state.musicusState.volume,
		trackSource: state.musicusState.currentTrack.trackSource
	}
}

export default connect(mapStateToProps, { setVolume })(VolumeBar)
