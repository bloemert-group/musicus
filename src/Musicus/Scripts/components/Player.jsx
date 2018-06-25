import React, { PropTypes } from 'react'
import { connect } from 'react-redux'
import Ink from 'react-ink';
import { next, play, pause, setJingles, playJingle } from '../actions/generalActions.jsx'
import Modal from '../components/Modal.jsx';
import Jingle from '../components/Jingle.jsx';

class Player extends React.Component {
	constructor(props) {
		super(props);
		this.clickPlay = this.clickPlay.bind(this);
		this.clickNext = this.clickNext.bind(this);
		this.clickPlayJingle = this.clickPlayJingle.bind(this);
		this.toggleJingleModal = this.toggleJingleModal.bind(this);

		this.state = { jingleModalIsOpen: false };

		this.props.setJingles();
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

	clickPlayJingle(filepath) {
		this.props.playJingle(filepath);
	}

	toggleJingleModal() {
		this.setState({
			jingleModalIsOpen: !this.state.jingleModalIsOpen
		});
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
				<div onClick={this.toggleJingleModal} className="button">
					<i className="icon step bullhorn" style={btnStyle}></i>
					<Ink />
				</div>
				<div onClick={this.clickPlay} className="button">
					<i className={icon} style={btnStyle}></i>
					<Ink />
				</div>
				<div onClick={this.clickNext} className="button">
					<i className='icon step forward' style={btnStyle}></i>
					<Ink />
				</div>
				<Modal show={this.state.jingleModalIsOpen} headerTitle="Jingles" onClose={this.toggleJingleModal}>
					{
						this.props.jingles.map(line =>
							<Jingle
								filepath={line.path}
								name={line.name}
								onclick={this.clickPlayJingle} />
						)
					}
				</Modal>
			</div>
		)
	}
}

Player.propTypes = {

}

function mapStateToProps(state) {
	return {
		isplaying: state.musicusState.isplaying,
		currentTrack: state.musicusState.currentTrack,
		jingles: state.musicusState.jingles
	}
}

export default connect(mapStateToProps, { next, play, pause, setJingles, playJingle })(Player)
