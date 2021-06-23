﻿using APICatalogoJogos.InputModel;
using APICatalogoJogos.Services;
using APICatalogoJogos.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogoJogos.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {

        private readonly IJogoService _jogoService;

        public JogosController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<JogoViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1,50)] int quantidade = 5)
        {
            var jogos = await _jogoService.Obter(pagina, quantidade);
            if (jogos.Count() == 0)
            {
                return NoContent();
            }
            return Ok(jogos);
        }

        [HttpGet("{idJogo:guid}")]
        public async Task<ActionResult<JogoViewModel>> Obter([FromRoute]Guid idJogo)
        {
            var jogo = await _jogoService.Obter(idJogo);
            if (jogo == null)
            {
                return NoContent();
            }

            return Ok(jogo);
        }

        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> InserirJogo([FromBody]JogoInputModel jogoInputModel)
        {
            try
            {
                var jogo = await _jogoService.Inserir(jogoInputModel);
                return Ok(jogo);

            }
            //catch (JogoJaCadastroException ex)
            catch (Exception ex)
            {

                return UnprocessableEntity("Já Existe um jogo com este nome para esta produtora");
            }
        }

        [HttpPut("{idJogo:guid}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute]Guid idJogo, [FromBody]JogoInputModel jogoInputModel)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, jogoInputModel);

                return Ok();
            }
            //catch (JogoNaoCadastradoException ex)
            catch (Exception ex)
            {

                return NotFound("Não existe esse jogo");
            }
            
        }

        [HttpPatch("{idJogo:guid}/preco/{preco:double}")]
        public async Task<ActionResult> AtualizaJogo([FromRoute]Guid idJogo, [FromRoute]double preco)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, preco);
                return Ok();
            }
            //catch (JogoNaoCadastradoException ex)
            catch (Exception ex)
            {

                return NotFound("Não existe esse Jogo");
            }
            
        }

        [HttpDelete("{idJogo:guid}")]
        public async Task<ActionResult> ApagarJogo(Guid idJogo)
        {
            return Ok();
        }
    }
}
