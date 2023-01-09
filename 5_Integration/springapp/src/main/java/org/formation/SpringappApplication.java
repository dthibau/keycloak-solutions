package org.formation;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.Bean;
import org.springframework.security.config.web.server.ServerHttpSecurity;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.annotation.AuthenticationPrincipal;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.oauth2.core.OAuth2AccessToken;
import org.springframework.security.oauth2.core.oidc.user.OidcUser;
import org.springframework.security.web.server.SecurityWebFilterChain;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

@SpringBootApplication
@RestController
public class SpringappApplication {

	public static void main(String[] args) {
		SpringApplication.run(SpringappApplication.class, args);
	}

	@Bean
	public SecurityWebFilterChain securitygWebFilterChain(ServerHttpSecurity http) {
	    return http.authorizeExchange()
	      .pathMatchers("/oidc-principal").hasAuthority("OIDC_USER")
	      .anyExchange().authenticated()
	      .and().oauth2Login()
	      .and().csrf().disable()
	      .build();
	}
	
	@GetMapping("/oidc-principal")
	public OidcUser getOidcUserPrincipal(
	  @AuthenticationPrincipal OidcUser principal) {
		Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
		
	    return principal;
	}
	
}
